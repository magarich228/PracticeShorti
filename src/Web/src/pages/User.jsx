import React, {useContext, useState, useEffect} from 'react'
import NavBar from '../components/NavBar/NavBar'
import { useParams } from 'react-router-dom'
import { AuthContext } from '../context'
import VideoPlayer from '../components/VideoPlayer/VideoPlayer';
import Search from '../components/Search/Search';
import VideoData from '../components/VideoData/VideoData';
import ShortsService from '../API/shortsService';

export default function Profile() {
    const {id} = useParams();
    const {curUserData} = useContext(AuthContext);
    const [videos, setVideos] = useState([]); 
    const [curVideo, setCurVideo] = useState(0);
    const {tokens} = useContext(AuthContext);

    useEffect(() => {
        if (curUserData) {
            (async () => {
                const res = await ShortsService.getUserShorts(tokens.accessToken, id);
                console.log("profile", res);
                if (res.ok && res.status === 200) {
                    const json = await res.json();
                    console.log("profile", json);
        
                    setVideos(json);
                }
            })();
        }
    }, [curUserData, id]);

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
                    <VideoData subscribeBtn={!(id === curUserData.id)} curVideo={videos[curVideo]} />
                </div>
            </aside>
        </div>
    )
}
