import React, { useContext } from 'react'
import { useEffect, useRef, useState } from "react";
import { AuthContext } from '../../context';
import s from './VideoPlayer.module.css'
import VolumeIcon from '../UI/VolumeIcon/VolumeIcon'
import PlayIcon from '../UI/PlayIcon/PlayIcon';
import LikeIcon from '../UI/LikeIcon/LikeIcon';
import ArrowDown from '../UI/ArrowDown/ArrowDown';
import ArrowUp from '../UI/ArrowUp/ArrowUp';
import ActivitiesService from '../../API/activitiesService';

export default function VideoPlayer({videos, curVideo, setCurVideo, children, like, preview = false}) {
    const feedEl = useRef();
    const isFirstVid = useRef(true);
    const [isVideosMuted, setIsVideosMuted] = useState(true);
    const [isPaused, setIsPaused] = useState(false);
    const {curUserData, tokens} = useContext(AuthContext);

    const [likesOnCurVideo, setLiketOnCurVideo] = useState(0);
    const [curUserLikes, setCurUserLikes] = useState([]);

    console.log("videos", videos);

    useEffect(() => {
        if (curUserData) {
            (async () => {
                const res = await ActivitiesService.getUserLikes(tokens.accessToken, curUserData.id);
                console.log("liked User videos", res);
                const json = await res.json();
                console.log("liked User videos", json);

                setCurUserLikes(json.map(el => el.shortId));
            })();
        }
    }, [curUserData]);

    useEffect(() => {
        if (like && videos.length) {
            (async () => {
                const res = await ActivitiesService.getCountLikes(tokens.accessToken, videos[curVideo].id);
                console.log("likes on video", res);
                setLiketOnCurVideo(res);
            })();
        }
    }, [like, videos, curVideo]);
  
    useEffect(() => {
        if (!videos.length) return;

        const video = document.querySelector("." + s.video+curVideo);
        const h = document.querySelector("." + s.video_block).clientHeight;

        if (!isFirstVid.current && !isPaused) {
            video.play();
        } 

        if (!isFirstVid.current && !isVideosMuted) {
            video.muted = false;
        }

        isFirstVid.current = false;

        feedEl.current.style.transform = `translateY(-${h * curVideo}px)`;

    }, [curVideo, isVideosMuted, isPaused]);

    if (!videos.length) {
        return (
            <div className={s.video_wrapper}>
                <div className={s.videos_container}>
                    <div className={s.feed} ref={feedEl}>
                        <h3 className={s.h3}>Пусто</h3>
                    </div>
                </div>
            </div>
        );
    }

    
    async function likeVideo(e) {
        if (curUserLikes.includes(videos[curVideo].id)) {
            const res = await ActivitiesService.unlikeVideo(tokens.accessToken, videos[curVideo].id);
            console.log("unlike vid", res);
            const json = await res.text();
            console.log("unlike vid", json);

            setCurUserLikes(curUserLikes.filter((shortId) => shortId != videos[curVideo].id));
            setLiketOnCurVideo(Number(likesOnCurVideo) - 1);
        } else {
            const res = await ActivitiesService.likeVideo(tokens.accessToken, videos[curVideo].id);
            console.log("like vid", res);
            const json = await res.json();
            console.log("like vid", json);
            
            setCurUserLikes([...curUserLikes, videos[curVideo].id]);
            setLiketOnCurVideo(Number(likesOnCurVideo) + 1);
        }
    }

    function next() {
        if (curVideo < videos.length - 1) {
            document.querySelector("."+ s.video + (curVideo)).pause();
            setCurVideo(curVideo + 1);
        }
    }

    function prev() {
        if (curVideo > 0) {
            document.querySelector("."+ s.video + (curVideo)).pause();
            setCurVideo(curVideo - 1);
        }
    }

    const muteVideo = (e) => {
        const currentVideo = document.querySelector("."+s.video+curVideo);

        currentVideo.muted = !currentVideo.muted;
        setIsVideosMuted(!isVideosMuted);
    };

    const pauseVideo = (e) => {
        const currentVideo = document.querySelector("."+s.video+curVideo);

        setIsPaused(!isPaused);

        if (isPaused) {
            currentVideo.play();
        } else {
            currentVideo.pause();
        }
    };

    return (
        <div className={s.video_wrapper}>
            <div className={s.videos_container}>
                <div className={s.feed} ref={feedEl}>
                    {
                    videos.map((vid, index) =>
                        <div className={s.video_block} key={vid.id}>
                            {
                            index === curVideo ? 
                                <video className={s.video + " " + s.video + index} autoPlay muted playsInline loop preload="auto" src={(preview ? 'http://localhost:3000/' : 'http://localhost:5171/')+vid.fileName}></video> 
                                    :
                                <video className={s.video + " " + s.video + index} loop preload="auto" src={(preview ? 'http://localhost:3000/' : 'http://localhost:5171/')+vid.fileName}></video> 
                            }
                        </div>) 
                    }
                </div>
            </div>

            <div className={s.pauseBtn} onClick={pauseVideo}>
                <button>
                    <PlayIcon isPause={isPaused} />
                </button>
            </div>

            {children}

            {
                like &&
                <div className={s.likeBtn} onClick={likeVideo}>
                    {likesOnCurVideo}
                    <button>
                        <LikeIcon liked={curUserLikes.includes(videos[curVideo].id)}/>
                    </button>
                </div>
            }

            <div className={s.mutedBtn}>
                <button onClick={muteVideo}>
                    <VolumeIcon isMuted={isVideosMuted} />    
                </button>
            </div>

            <div className={s.NextPrevBtns}>
                <button onClick={prev}>
                    <ArrowUp />
                </button>
                <button onClick={next}>
                    <ArrowDown />
                </button>
            </div>
        </div>
    )
}
