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

    useEffect(() => {
        if (curUserData) {
            (async () => {
                const res = await ShortsService.getLikedShorts(tokens.accessToken, curUserData.id);
                console.log("like", res);
                const json = await res.json();
                console.log("like", json);

                setVideos(json);
            })();
        }
    }, [curUserData]);

    return (
        <div className="LikesPage">
            <main className="PlayerContainer">
                <VideoPlayer 
                    videos={videos} 
                    curVideo={curVideo} 
                    setCurVideo={(curIndex) => setCurVideo(curIndex)} 
                    like={true}
                    preview={false}
                >
                    <Search />
                </VideoPlayer>
            </main>
            <aside className="LikesAside">
                <nav className="LikesNav">
                    <NavBar />
                </nav>
                <div className="AsideContent">
                    <VideoData subscribeBtn={true} curVideo={videos[curVideo]} />
                </div>
            </aside>
        </div>
    )
}
