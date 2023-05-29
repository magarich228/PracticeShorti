import React, {useContext, useState, useEffect} from 'react'
import NavBar from '../components/NavBar/NavBar'
import { useParams } from 'react-router-dom'
import { AuthContext } from '../context'
import VideoPlayer from '../components/VideoPlayer/VideoPlayer';
import Search from '../components/Search/Search';
import VideoData from '../components/VideoData/VideoData';
import ShortsService from '../API/shortsService';
import UserService from '../API/userService';

export default function Profile() {
    const {id} = useParams();
    const {curUserData, tokens} = useContext(AuthContext);
    const [videos, setVideos] = useState([]); 
    const [curVideo, setCurVideo] = useState(0);
    const [targetUser, setTargetUser] = useState({});

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

    useEffect(() => {
        if (id) {
            (async () => {
                const res = await UserService.getById(tokens.accessToken, id);
                const json = await res.json();

                setTargetUser(json);
            })();
        }
    }, [id]);

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
                    <VideoData user={targetUser} subscribeBtn={true} curVideo={videos[curVideo]} />
                </div>
            </aside>
        </div>
    )
}
