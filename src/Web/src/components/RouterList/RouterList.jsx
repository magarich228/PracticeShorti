import React, { useContext, useEffect } from 'react'
import {Routes, Route, Navigate} from 'react-router-dom'
import Main from '../../pages/Main'
import VideoAdd from '../../pages/VideoAdd'
import LogIn from '../../pages/LogIn'
import PageNotFound from '../../pages/PageNotFound'
import { AuthContext } from '../../context'

export default function RouterList() {
    const {isAuth} = useContext(AuthContext);

    console.log(isAuth);
    
    return (
        isAuth ?
            <Routes>
                <Route path="/" element={<Main />} />
                <Route path="/newvideo" element={<VideoAdd />} />
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
