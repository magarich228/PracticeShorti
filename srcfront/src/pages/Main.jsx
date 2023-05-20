import React from 'react'
import { useState } from 'react';
import VideoPlayer from '../components/VideoPlayer/VideoPlayer';

export default function Main() {
    const videos = ["/short4.mp4", "/short2.mp4", "/short3.mp4", "/short1.mp4", "/video1.mp4", "/video2.mp4", "/video3.mp4"];
    const [curVideo, setCurVideo] = useState(0);

    return (
        <div>
            <VideoPlayer videos={videos} curVideo={curVideo} setCurVideo={(curIndex) => setCurVideo(curIndex)} />
        </div>
    )
}
