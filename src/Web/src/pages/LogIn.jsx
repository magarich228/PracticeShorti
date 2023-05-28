import React from 'react'
import LoginForm from '../components/LoginForm/LoginForm';

export default function LogIn() {
    return (
        <div className="loginContainer">
            <div className='logo_container'>
                <img src="http://localhost:3000/images/Logo.png" alt="" />
            </div>
            <LoginForm />
        </div>
    )
}
