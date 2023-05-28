import React, { useContext, useEffect, useState } from 'react'
import s from "./VideoData.module.css"
import UserData from '../UserData/UserData'
import UserService from '../../API/userService';
import { AuthContext } from '../../context';
import ActivitiesService from '../../API/activitiesService';

export default function VideoData({curVideo, subscribeBtn}) {
    const {tokens} = useContext(AuthContext);
    const [curVideoUser, setCurVideoUser] = useState(null);
    const [countSubsAuthor, setCountSubsAuthor] = useState(0);
    
    useEffect(() => {
        if (curVideo) {
            (async () => {
                const res = await UserService.getById(tokens.accessToken, curVideo.authorId);
                console.log("video user", res);
                const json = await res.json();
                console.log("video user", json);

                setCurVideoUser(json);
            })();
        }
    }, [curVideo]);

    useEffect(() => {
        if (curVideoUser) {
            (async () => {
                const count = await ActivitiesService.getCountSubs(tokens.accessToken, curVideoUser.id);
                console.log("user subs count", count);

                setCountSubsAuthor(count);
            })();
        }
    }, [curVideoUser]);

    if (!curVideo) {
        return (
            <div className={s.container}>
                <UserData subscribeBtn={true} user={curVideoUser}>
                    <span>{"Дата"}</span>
                </UserData>
                <div className={s.desc}>
                    <div className={s.descText}>
                        <h3>{"Заголовок"}</h3>
                        <p>{"Описание"}</p>
                    </div>
                </div>
            </div>
        );
    }

    return (
        <div className={s.container}>
            <UserData subscribeBtn={subscribeBtn} user={curVideoUser}>
                <span>{curVideo.uploadedAt}</span>
            </UserData>
            <div className={s.desc}>
                <div className={s.descText}>
                    <h3>{curVideo.title}</h3>
                    <p>{curVideo.description}</p>
                </div>
                {/* <ul className={s.tags}>
                    <li className={s.tag}>#{"тег"}</li>
                    <li className={s.tag}>#{"тег"}</li>
                    <li className={s.tag}>#{"тег"}</li>
                    <li className={s.tag}>#{"тег"}</li>
                    <li className={s.tag}>#{"тег"}</li>
                </ul> */}
            </div>
            <div className={s.subs}>
                Подписчиков: <strong>{countSubsAuthor}</strong>
            </div>
        </div>
    )
}
