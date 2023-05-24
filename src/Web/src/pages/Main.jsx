import React from 'react'
import { useState } from 'react';
import VideoPlayer from '../components/VideoPlayer/VideoPlayer';
import NavBar from '../components/NavBar/NavBar';
import VideoData from '../components/VideoData/VideoData';
import Search from '../components/Search/Search';

export default function Main() {
    const videos = ["/short4.mp4", "/short2.mp4", "/short3.mp4", "/short1.mp4", "/video1.mp4", "/video2.mp4", "/video3.mp4"];
    const [curVideo, setCurVideo] = useState(0);

    return (
        <div className="MainPage">
            <main className="PlayerContainer">
                <VideoPlayer 
                    videos={videos} 
                    curVideo={curVideo} 
                    setCurVideo={(curIndex) => setCurVideo(curIndex)} 
                    like={true}
                >
                    <Search />
                </VideoPlayer>
            </main>
            <aside className="MainAside">
                <nav className="MainNav">
                    <NavBar />
                </nav>
                <div className="AsideContent">
                    <VideoData curVideo={videos[curVideo]} />
                </div>
            </aside>
        </div>
    )
}
