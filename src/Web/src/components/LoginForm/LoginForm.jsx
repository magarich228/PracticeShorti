import React, { useContext, useEffect, useState } from 'react'
import { AuthContext } from '../../context';
import s from './LoginForm.module.css'

export default function LoginForm() {
    const {setIsAuth} = useContext(AuthContext);
    const [curForm, setCurForm] = useState("auth");

    function onChange(e) {
        setCurForm(e.target.value);
    }

    function submit(e) {
        e.preventDefault();
        setIsAuth(true);
    } 

    return (
        <div className={s.container}>
            <div>
                <label className={s.label + " " + (curForm == "auth" && s.active)} htmlFor='authradio'>Авторизация</label>
                <input onChange={onChange} className={s.radio} type="radio" name="authradio" id='authradio' value={"auth"} />
                <span className={s.devider}> / </span>
                <label className={s.label + " " + (curForm == "reg" && s.active)} htmlFor='regradio'>Регистрация</label>
                <input onChange={onChange} className={s.radio} type="radio" name="authradio" id='regradio' value={"reg"}/>
            </div>
            {curForm == "auth" ?
                <form onSubmit={submit} className={s.form}>
                    <input type="text" placeholder='login' />
                    <input type="text" placeholder='pass' />
                    <button>Авторизоваться</button>
                </form> 
                    :
                <form onSubmit={submit} className={s.form}>
                    <input type="text" placeholder='login' />
                    <input type="text" placeholder='pass' />
                    <label>
                        <span>Аватарка: </span>
                        <input type="file" />
                    </label>
                    <button>Зарегестрироваться</button>
                </form>
            }
        </div>
    )
}
