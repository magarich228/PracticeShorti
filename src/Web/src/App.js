import { BrowserRouter} from "react-router-dom";
import "normalize.css"
import "./main.css"
import RouterList from "./components/RouterList/RouterList";
import { AuthContext } from "./context";
import { useEffect, useState } from "react";
import LoginService from "./API/loginService";
import { getCookie, setCookie } from "./utils/cookies";
import { useAuthorization } from "./hooks/authHooks";

function App() {
    const [isAuth, setIsAuth, tokens, setTokens, curUserData, setCurUserData] = useAuthorization();

    return (
        <AuthContext.Provider value={{isAuth, setIsAuth, tokens, setTokens, curUserData, setCurUserData}}>
            <BrowserRouter>
                <div className="App">
                    <RouterList />
                </div>
            </BrowserRouter>
        </AuthContext.Provider>
    );
}

export default App;
