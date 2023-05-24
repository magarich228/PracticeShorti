import React from 'react'
import { useState } from 'react';
import VideoPlayer from '../components/VideoPlayer/VideoPlayer';
import NavBar from '../components/NavBar/NavBar';
import VideoData from '../components/VideoData/VideoData';
import Search from '../components/Search/Search';
import VideoDescEdit from '../components/VideoDescEdit/VideoDescEdit';

export default function VideoAdd() {
    // const videos = ["/short4.mp4", "/short2.mp4", "/short3.mp4", "/short1.mp4", "/video1.mp4", "/video2.mp4", "/video3.mp4"];
    const [videos, setVideos] = useState([]);
    const [curVideo, setCurVideo] = useState(0);

    return (
        <div className="VideoAddPage">
            <main className="PlayerContainer">
                <VideoPlayer 
                    videos={videos} 
                    curVideo={curVideo} 
                    setCurVideo={(curIndex) => setCurVideo(curIndex)} 
                ></VideoPlayer>
            </main>
            <aside className="VideoAddAside">
                <nav className="VideoAddNav">
                    <NavBar />
                </nav>
                <div className="AsideContent">
                    <VideoDescEdit onVideoLoad={(video) => {
                        setVideos([video]);
                    }}/>
                </div>
            </aside>
        </div>
    )
}
