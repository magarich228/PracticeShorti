import React, { useState } from 'react'
import s from "./Search.module.css"
import { useNavigate } from 'react-router-dom';

export default function Search() {
    const [query, setQuery] = useState("");
    const navigate = useNavigate();

    function find(e) {
        console.log("query", query);
        navigate(`/search/${query}`, {replace: true});
    }

    return (
        <div className={s.container}>
            <input value={query} onChange={(e) => setQuery(e.target.value)} type="text" className={s.searchInput} placeholder='Поиск...' />
            <button className={s.btn} type='button' onClick={find}>
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-search" viewBox="0 0 16 16">
                    <path d="M11.742 10.344a6.5 6.5 0 1 0-1.397 1.398h-.001c.03.04.062.078.098.115l3.85 3.85a1 1 0 0 0 1.415-1.414l-3.85-3.85a1.007 1.007 0 0 0-.115-.1zM12 6.5a5.5 5.5 0 1 1-11 0 5.5 5.5 0 0 1 11 0z"/>
                </svg>
            </button>
        </div>
    )
}
