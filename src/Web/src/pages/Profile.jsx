import React, { useContext, useEffect, useRef, useState } from 'react'
import NavBar from '../components/NavBar/NavBar'
import { AuthContext } from '../context'
import VideoPlayer from '../components/VideoPlayer/VideoPlayer';
import Search from '../components/Search/Search';
import VideoData from '../components/VideoData/VideoData';
import ShortsService from '../API/shortsService';
import UserService from '../API/userService';

export default function Profile() {
    const {curUserData} = useContext(AuthContext);
    const [videos, setVideos] = useState([]); 
    const [curVideo, setCurVideo] = useState(0);
    const {tokens} = useContext(AuthContext);
    const avatarForm = useRef();

    console.log(avatarForm);

    useEffect(() => {
        if (curUserData) {
            (async () => {
                const res = await ShortsService.getUserShorts(tokens.accessToken, curUserData.id);
                console.log("profile", res);
                if (res.ok && res.status === 200) {
                    const json = await res.json();
                    console.log("profile", json);
        
                    setVideos(json);
                }
            })();
        }
    }, [curUserData]);

    function onAvatarSend(e) {
        e.preventDefault();

        (async () => {
            const res = await UserService.avatarUpdate(tokens.accessToken, avatarForm.current)
            console.log("avatar up", res);
            const json = await res.json();
            console.log("avatar up", json);
        })();
    }

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
                    <VideoData subscribeBtn={false} curVideo={videos[curVideo]} />
                    <form ref={avatarForm} method='PUT' onSubmit={onAvatarSend}>
                        <input name='avatar' type="file" accept='image/png, image/jpeg' />
                        <button type='submit'>Загрузить аватарку</button>
                    </form>
                </div>
            </aside>
        </div>
    )
}
