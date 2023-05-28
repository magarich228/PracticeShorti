import React, { useContext, useEffect, useState } from 'react'
import s from './UserData.module.css'
import ActivitiesService from '../../API/activitiesService';
import { Link } from 'react-router-dom';
import { AuthContext } from '../../context';
import { useId } from 'react';

export default function UserData({children, subscribeBtn, user}) {
    const {curUserData, tokens} = useContext(AuthContext);
    const [curUserSubscriptions, setCurUserSubscriptions] = useState([]);

    useEffect(() => {
        if (curUserData) {
            (async () => {
                const res = await ActivitiesService.getUserSubscriptions(tokens.accessToken, curUserData.id);
                console.log("curUser Subs", res);
                const json = await res.json();
                console.log("curUser Subs", json);

                setCurUserSubscriptions(json.map(el => el.subscriptionId));
            })();
        }
    }, [curUserData]);

    useEffect(() => {
        if (user && curUserData) {

        }
    }, [user, curUserData]);

    async function subscribe(e) {
        if (curUserSubscriptions.includes(user.id)) {
            const res = await ActivitiesService.unsubsribe(tokens.accessToken, user.id);
            console.log("unsubs", res);
            const json = await res.text();
            console.log("unsubs", json);

            setCurUserSubscriptions(curUserSubscriptions.filter((usrId) => usrId != user.id));
        } else {
            const res = await ActivitiesService.subscribe(tokens.accessToken, user.id);
            console.log("subs", res);
            const json = await res.json();
            console.log("subs", json);

            setCurUserSubscriptions([...curUserSubscriptions, user.id]);
        }
    } 

    if (!user) {
        return (
            <div className={s.container}>
                <div className={s.inner}>
                    <div className={s.avatar_box}>
                        <div className={s.img}>
                            <img src="/images/user.png" alt="usericon" />
                        </div>
                    </div>
                    <div className={s.data}>
                        <div className={s.text}>
                            <h3>
                                Имя пользователя
                            </h3>
                            <div>
                                {children}
                            </div>
                        </div>
                        <div className={s.btn}>
                            { subscribeBtn && 
                                <button>Подписаться</button>
                            }
                        </div>
                    </div>
                </div>
            </div>
        );
    }

    return (
        <div className={s.container}>
            <div className={s.inner}>
                <div className={s.avatar_box}>
                    <div className={s.img}>
                        <img src={'http://localhost:5171/'+user.avatarPath} alt="usericon" />
                    </div>
                </div>
                <div className={s.data}>
                    <div className={s.text}>
                        <h3>
                            <Link className={s.link} to={`/profile/${user.id}`}>
                                {user.userName}
                            </Link>
                        </h3>
                        <div>
                            {children}
                        </div>
                    </div>
                    <div className={s.btn}>
                        { subscribeBtn && 
                            (!(user.id === (curUserData ? curUserData.id : "")) ? <button onClick={subscribe}>{curUserSubscriptions.includes(user.id) ? "Отписаться" : "Подписаться"}</button> : "")
                        }
                    </div>
                </div>
            </div>
        </div>
    )
}
