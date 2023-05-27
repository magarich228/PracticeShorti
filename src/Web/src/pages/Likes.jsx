import React, { useEffect } from 'react'
import { useState, useContext } from 'react';
import VideoPlayer from '../components/VideoPlayer/VideoPlayer';
import NavBar from '../components/NavBar/NavBar';
import VideoData from '../components/VideoData/VideoData';
import Search from '../components/Search/Search';
import { AuthContext } from '../context';
import { useRefreshToken } from '../hooks/authHooks';
import ShortsService from '../API/shortsService';

export default function Likes() {
    const [videos, setVideos] = useState([]); 
    const [curVideo, setCurVideo] = useState(0);
    const {tokens, curUserData} = useContext(AuthContext);
    const getShorts = useRefreshToken(ShortsService.getLikedShorts);

    useEffect(() => {
        (async () => {
            const res = await getShorts(tokens.accessToken, curUserData.id);
            console.log(res);
            const json = res.json();
            console.log(json);
        })();
    }, []);

    return (
        <div className="LikesPage">
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
            <aside className="LikesAside">
                <nav className="LikesNav">
                    <NavBar />
                </nav>
                <div className="AsideContent">
                    <VideoData curVideo={videos[curVideo]} />
                </div>
            </aside>
        </div>
    )
}
