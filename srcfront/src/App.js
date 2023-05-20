import { BrowserRouter} from "react-router-dom";
import "./main.css"
import "normalize.css"
import RouterList from "./components/RouterList/RouterList";
import { AuthContext } from "./context";
import { useState } from "react";

function App() {
    const [isAuth, setIsAuth] = useState(false);

    return (
        <AuthContext.Provider value={{isAuth, setIsAuth}}>
            <BrowserRouter>
                <div className="App">
                    <RouterList />
                </div>
            </BrowserRouter>
        </AuthContext.Provider>
    );
}

export default App;
