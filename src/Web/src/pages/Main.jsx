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
    const [videos, setVideos] = useState([]); 
    const [curVideo, setCurVideo] = useState(0);
    const {tokens} = useContext(AuthContext);
    const getShorts = useRefreshToken(ShortsService.getShorts);
    const [page, setPage] = useState(1);
    // const [totalCount, setTotalCount] = useState(1);
    const videosCount = 20;

    useEffect(() => {
        (async () => {
            const res = await getShorts(tokens.accessToken, page, videosCount);
            console.log("headers", res.headers["Count"]);
            const json = await res.json();
            console.log(json);

            setVideos(json);
        })();
    }, [page]);

    return (
        <div className="MainPage">
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
            <aside className="MainAside">
                <nav className="MainNav">
                    <NavBar />
                </nav>
                <div className="AsideContent">
                    <VideoData subscribeBtn={true} curVideo={videos[curVideo]} />
                </div>
            </aside>
        </div>
    )
}
