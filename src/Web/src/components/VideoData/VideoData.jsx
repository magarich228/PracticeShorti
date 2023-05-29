import React, { useContext, useEffect, useState } from 'react'
import s from "./VideoData.module.css"
import UserData from '../UserData/UserData'
import UserService from '../../API/userService';
import { AuthContext } from '../../context';
import ActivitiesService from '../../API/activitiesService';

export default function VideoData({curVideo, subscribeBtn, user}) {
    const {tokens} = useContext(AuthContext);
    const [countSubsAuthor, setCountSubsAuthor] = useState(0);


    useEffect(() => {
        if (user?.id) {
            (async () => {
                const count = await ActivitiesService.getCountSubs(tokens.accessToken, user.id);
                console.log("user subs count", count);

                setCountSubsAuthor(count);
            })();
        }
    }, [user]);

    if (!curVideo || !user) {
        return (
            <div className={s.container}>
                <UserData subscribeBtn={true} user={false}>
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
            <UserData subscribeBtn={subscribeBtn} user={user}>
                <span>{curVideo.uploadedAt}</span>
            </UserData>
            <div className={s.desc}>
                <div className={s.descText}>
                    <h3>{curVideo.title}</h3>
                    <p>{curVideo.description}</p>
                </div>
            </div>
            <div className={s.subs}>
                Подписчиков: <strong>{countSubsAuthor}</strong>
            </div>
        </div>
    )
}
