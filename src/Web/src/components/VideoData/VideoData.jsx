import React from 'react'
import s from "./VideoData.module.css"
import UserData from '../UserData/UserData'

export default function VideoData({curVideo}) {
  return (
    <div className={s.container}>
        <UserData subscribeBtn={true}>
            <span>Дата</span>
        </UserData>
        <div className={s.desc}>
            <div className={s.descText}>
                <h3>Unknown</h3>
                <p>{"текст текст текст текст текст текст текст текст текст текст текст текст текст текст текст текст текст текст текст текст текст текст текст текст текст текст текст текст текст текст "}</p>
            </div>
            <ul className={s.tags}>
                <li className={s.tag}>#{"тег"}</li>
                <li className={s.tag}>#{"тег"}</li>
                <li className={s.tag}>#{"тег"}</li>
                <li className={s.tag}>#{"тег"}</li>
                <li className={s.tag}>#{"тег"}</li>
            </ul>
        </div>
    </div>
  )
}
