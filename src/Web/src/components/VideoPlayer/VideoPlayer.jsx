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

    console.log("videos", videos);

    useEffect(() => {
        if (curUserData) {
            (async () => {
                // const res = await ActivitiesService.getUserSubscriptions(tokens.accessToken, curUserData.id);
                // console.log("liked User videos", res);
                // const json = await res.text();
                // console.log("liked User videos", json);
            })();
        }
    }, [curUserData]);
  
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

    
    function likeVideo(e) {

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
                    <button>
                        <LikeIcon liked={false}/>
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
