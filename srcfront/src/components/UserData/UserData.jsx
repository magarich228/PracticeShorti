import React from 'react'
import s from './UserData.module.css'

export default function UserData({children}) {
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
                    <h3>Имя пользователя</h3>
                    <div>
                        {children}
                    </div>
                </div>
                <div className={s.btn}>
                    <button>Подписаться</button>
                </div>
            </div>
        </div>
    </div>
  )
}
