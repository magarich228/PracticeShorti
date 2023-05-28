import React, {useState, useContext, useEffect} from 'react'
import { useParams } from 'react-router-dom';
import { AuthContext } from '../context';
import VideoPlayer from '../components/VideoPlayer/VideoPlayer';
import NavBar from '../components/NavBar/NavBar';
import VideoData from '../components/VideoData/VideoData';
import ShortsService from '../API/shortsService';
import UserService from '../API/userService';

export default function Search() {
    const [videos, setVideos] = useState([]); 
    const [curVideo, setCurVideo] = useState(0);
    const {tokens} = useContext(AuthContext);
    const {query} = useParams();

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
        if (query) {
            (async () => {
                const res = await ShortsService.findShorts(tokens.accessToken, query);
                console.log("search", res);
                const json = await res.json();
                console.log("search", json);

                setVideos(json);
            })();
        }
    }, []);

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
