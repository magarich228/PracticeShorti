import React, { useContext, useEffect } from 'react'
import { useState } from 'react';
import VideoPlayer from '../components/VideoPlayer/VideoPlayer';
import NavBar from '../components/NavBar/NavBar';
import VideoData from '../components/VideoData/VideoData';
import Search from '../components/Search/Search';
import UserService from '../API/userService';
import { AuthContext } from '../context';
import { useRefreshToken } from '../hooks/authHooks';
import ShortsService from '../API/shortsService';

export default function Main() {
    // const videos = ["/short4.mp4", "/short2.mp4", "/short3.mp4", "/short1.mp4", "/video1.mp4", "/video2.mp4", "/video3.mp4"];
    const [videos, setVideos] = useState([]); 
    const [curVideo, setCurVideo] = useState(0);
    const {tokens} = useContext(AuthContext);
    const getShorts = useRefreshToken(ShortsService.getShorts);

    useEffect(() => {
        (async () => {
            const res = await getShorts(tokens.accessToken, 1, 11);
            console.log(res);
            const json = await res.json();
            console.log(json);

            setVideos(json);
        })();
    }, []);

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
