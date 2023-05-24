import React from 'react'
import s from "./Search.module.css"

export default function Search() {
  return (
    <div className={s.container}>
        <input type="text" className={s.searchInput} placeholder='Поиск...' />
    </div>
  )
}
