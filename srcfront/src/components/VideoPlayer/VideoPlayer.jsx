import React from 'react'
import { useEffect, useRef, useState } from "react";
import s from './VideoPlayer.module.css'
import VolumeIcon from '../UI/VolumeIcon'

export default function VideoPlayer({videos, curVideo, setCurVideo, children, like}) {
    const feedEl = useRef();
    const isFirstVid = useRef(true);
    const [isVideosMuted, setIsVideosMuted] = useState(true);

    useEffect(() => {
        const video = document.querySelector("." + s.video+curVideo);
        const h = document.querySelector("." + s.video_block).clientHeight;

        if (!isFirstVid.current) {
            video.play();
            
            if (!isVideosMuted) {
                video.muted = false;
            }
        } 

        isFirstVid.current = false;

        feedEl.current.style.transform = `translateY(-${h * curVideo}px)`;

    }, [curVideo, isVideosMuted]);

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

        if (isFirstVid.current) currentVideo.play();

        currentVideo.muted = !currentVideo.muted;
        setIsVideosMuted(!isVideosMuted);
    };

    return (
        <div className={s.video_wrapper}>
            <div className={s.videos_container}>
                <div className={s.feed} ref={feedEl}>
                    { 
                    videos.map((vid, index) =>
                        <div className={s.video_block} key={vid}>
                            {
                            index === curVideo ? 
                                <video className={s.video + " " + s.video + index} autoPlay muted playsInline loop preload="auto" src={vid}></video> 
                                    :
                                <video className={s.video + " " + s.video + index} loop preload="auto" src={vid}></video> 
                            }
                        </div>) 
                    }
                </div>
            </div>
            {children}
            {
                like &&
                <div></div>
            }

            <div className={s.mutedBtn}>
                <button onClick={muteVideo}>
                    <VolumeIcon isMuted={isVideosMuted} />    
                </button>
            </div>

            <div className={s.pauseBtn}>
                <button>
                    Пауза
                </button>
            </div>

            <div className={s.NextPrevBtns}>
                <button onClick={prev}>Prev</button>
                <button onClick={next}>Next</button>
            </div>

            {/* <div className={s.btns}>
                <button onClick={prev}>Prev</button>
                <button onClick={next}>Next</button>
                <button onClick={muteVideo}>
                    <VolumeIcon isMuted={isVideosMuted} />    
                </button>
            </div> */}
        </div>
    )
}
