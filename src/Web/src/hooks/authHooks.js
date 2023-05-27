import { useContext, useEffect, useState } from "react";
import { setCookie, getCookie } from "../utils/cookies";
import LoginService from "../API/loginService";
import UserService from "../API/userService";
import { AuthContext } from "../context";

export function useAuthorization() {
    const authCookie = getCookie("tokens");
    const [isAuth, setIsAuth] = useState(!!authCookie);
    const [tokens, setTokens] = useState(authCookie ? JSON.parse(authCookie) : null);
    const [curUserData, setCurUserData] = useState(null); 

    useEffect(() => {
        if (tokens) {
            setCookie("tokens", JSON.stringify(tokens), {secure: true, 'max-age': "3600"});
        }
    }, [tokens]);

    useEffect(() => {
        if (tokens) {
            (async () => {
                const res = await UserService.getCur(tokens.accessToken);
                console.log("res curUser", res);
                const json = await res.json();
                console.log("json curUser", json);

                setCurUserData(json);
            })();
        }
    }, [tokens]);

    return [isAuth, setIsAuth, tokens, setTokens, curUserData];
}

export function useRefreshToken(requestFunc) {
    const {tokens, setTokens} = useContext(AuthContext);

    return async (...args) => {
        const ret = await requestFunc(...args);
    
        const res = await LoginService.refreshToken(tokens.refreshToken, tokens.accessToken);
        const json = await res.json();
    
        console.log("refresh hook", res);
        console.log("refresh hook", json);
    
        if (res.ok) {
            setTokens({accessToken: json.accessToken, refreshToken: json.refreshToken});
        }
    
        return ret;
    }; 
}