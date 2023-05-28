import React, { useContext, useEffect, useState } from 'react'
import { AuthContext } from '../../context';
import s from './LoginForm.module.css'
import LoginService from '../../API/loginService';

export default function LoginForm() {
    const {setIsAuth, setTokens} = useContext(AuthContext);
    const [curForm, setCurForm] = useState("auth");
    const [errMes, setErrMes] = useState("");

    const [authData, setAuthData] = useState({
        userName: "",
        password: ""
    });
    const [regData, setRegData] = useState({
        userName: "",
        password: "",
        confirmPassword: ""
    });

    function submitAuth(e) {
        e.preventDefault();

        const req = async () => {
            const res = await LoginService.signin(authData);
            const json = await res.json();

            // console.log(res);
            // console.log(json);

            if (res.ok) {
                setTokens({accessToken: json.accessToken, refreshToken: json.refreshToken});
                setIsAuth(true);
                setErrMes("");
            } else {
                setErrMes("Ошибка");
            }
        };

        req();
    } 

    function submitReg(e) {
        e.preventDefault();

        const req = async () => {
            const res = await LoginService.signup(regData);
            const json = await res.json();

            if (res.ok) {
                setTokens({accessToken: json.accessToken, refreshToken: json.refreshToken});
                setIsAuth(true);
                setErrMes("");
            } else {
                setErrMes("ошибка");
            }
        }; 

        req();
    }

    return (
        <div className={s.container}>
            <div style={{color: "red"}}>
                {errMes}
            </div>
            <div>
                <label className={s.label + " " + (curForm == "auth" && s.active)} htmlFor='authradio'>Авторизация</label>
                <input onChange={(e) => setCurForm(e.target.value)} className={s.radio} type="radio" name="authradio" id='authradio' value={"auth"} />
                <span className={s.devider}> / </span>
                <label className={s.label + " " + (curForm == "reg" && s.active)} htmlFor='regradio'>Регистрация</label>
                <input onChange={(e) => setCurForm(e.target.value)} className={s.radio} type="radio" name="authradio" id='regradio' value={"reg"}/>
            </div>
            {curForm == "auth" ?
                <form onSubmit={submitAuth} className={s.form}>
                    <input value={authData.userName} onInput={(e) => setAuthData({...authData, userName: e.target.value})} type="text" placeholder='login' />
                    <input value={authData.password} onInput={(e) => setAuthData({...authData, password: e.target.value})} type="text" placeholder='pass' />
                    <button>Авторизоваться</button>
                </form> 
                    :
                <form onSubmit={submitReg} className={s.form}>
                    <input value={regData.userName} onInput={(e) => setRegData({...regData, userName: e.target.value})} type="text" placeholder='login' />
                    <input value={regData.password} onInput={(e) => setRegData({...regData, password: e.target.value})} type="text" placeholder='pass' />
                    <input value={regData.confirmPassword} onInput={(e) => setRegData({...regData, confirmPassword: e.target.value})} type="text" placeholder='confirmPass' />
                    <button>Зарегестрироваться</button>
                </form>
            }
        </div>
    )
}
