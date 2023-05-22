import React from 'react'
import s from "./VideoData.module.css"
import UserData from '../UserData/UserData'

export default function VideoData({curVideo}) {
  return (
    <div className={s.container}>
        <UserData>
            <span>Дата</span>
        </UserData>
        <div className={s.desc}>
            <p className={s.descText}>
                {"текст текст текст текст текст текст текст текст текст текст текст текст текст текст текст текст текст текст текст текст текст текст текст текст текст текст текст текст текст текст "}
            </p>
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
