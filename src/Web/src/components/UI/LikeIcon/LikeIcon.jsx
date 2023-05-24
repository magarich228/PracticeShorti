import React from 'react'
import s from './LikeIcon.module.css'

export default function LikeIcon({liked}) {
    return (
        <svg xmlns="http://www.w3.org/2000/svg" width="40" height="40" fill="currentColor" className={s.svg + (liked ? " " + s.liked : "")} viewBox="0 0 16 16">
            <path fillRule="evenodd" d="M8 1.314C12.438-3.248 23.534 4.735 8 15-7.534 4.736 3.562-3.248 8 1.314z"/>
        </svg>
    )
}
