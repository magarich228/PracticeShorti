import React, { useContext } from 'react'
import {Routes, Route, Navigate} from 'react-router-dom'
import Main from '../../pages/Main'
import VideoAdd from '../../pages/VideoAdd'
import LogIn from '../../pages/LogIn'
import PageNotFound from '../../pages/PageNotFound'
import { AuthContext } from '../../context'
import Profile from '../../pages/Profile'
import Likes from '../../pages/Likes'
import Subscribes from '../../pages/Subscribes'
import User from '../../pages/User'

export default function RouterList() {
    const {isAuth} = useContext(AuthContext);
    
    return (
        isAuth ?
            <Routes>
                <Route path="/" element={<Main />} />
                <Route path="/newvideo" element={<VideoAdd />} />
                <Route path="/likes" element={<Likes />} />
                <Route path="/subscribes" element={<Subscribes />} />
                <Route path="/profile" element={<Profile />} />
                <Route path="/profile/:id" element={<User />} />
                <Route path="/login" element={<Navigate to="/" replace/>} />
                <Route path="*" element={<PageNotFound />} />
            </Routes>
                :
            <Routes>
                <Route path="/login" element={<LogIn />} />
                <Route path="*" element={<Navigate to="/login" replace />} />
            </Routes>
        );
}
