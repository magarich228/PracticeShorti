import React, { useEffect } from 'react'
import s from './UserData.module.css'
import ActivitiesService from '../../API/activitiesService';
import { Link } from 'react-router-dom';

export default function UserData({children, subscribeBtn, user}) {

    useEffect(() => {
        if (user) {
            
        }
    }, [user]);

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
                            <button>Подписаться</button>
                        }
                    </div>
                </div>
            </div>
        </div>
    )
}
