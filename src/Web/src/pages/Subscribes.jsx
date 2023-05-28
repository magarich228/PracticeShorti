import React from 'react'
import { useState, useContext, useEffect } from 'react';
import VideoPlayer from '../components/VideoPlayer/VideoPlayer';
import NavBar from '../components/NavBar/NavBar';
import VideoData from '../components/VideoData/VideoData';
import Search from '../components/Search/Search';
import ShortsService from '../API/shortsService';
import { AuthContext } from '../context';
import UserService from '../API/userService';

export default function Subscribes() {
    const [videos, setVideos] = useState([]); 
    const [curVideo, setCurVideo] = useState(0);
    const {tokens, curUserData} = useContext(AuthContext);

    const [targetUser, setTargetUser] = useState({});

    useEffect(() => {
        if (videos.length) {
            (async () => {
                const res = await UserService.getById(tokens.accessToken, videos[curVideo].authorId);
                const json = await res.json();

                setTargetUser(json);
            })();
        }
    }, [curVideo, videos]);

    useEffect(() => {
        if (curUserData) {
            (async () => {
                const res = await ShortsService.getSubscriptionShorts(tokens.accessToken, curUserData.id);
                console.log("like", res);
                const json = await res.json();
                console.log("like", json);

                setVideos(json);
            })();
        }
    }, [curUserData]);

    return (
        <div className="SubsPage">
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
            <aside className="SubsAside">
                <nav className="SubsNav">
                    <NavBar />
                </nav>
                <div className="AsideContent">
                    <VideoData user={targetUser} subscribeBtn={true} curVideo={videos[curVideo]} />
                </div>
            </aside>
        </div>
    )
}
