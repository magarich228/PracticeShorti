import React, { useRef, useState } from 'react'
import s from './VideoDescEdit.module.css'
import UserData from '../UserData/UserData'

export default function VideoDescEdit({onVideoLoad}) {
    const [tags, setTags] = useState([]);
    const [newTag, setNewTag] = useState("");
    const [description, setDescription] = useState({name: "", desc: ""});

    function addTag(e) {
        if (!newTag) return;

        setTags([...tags, newTag]);
        setNewTag("");
    }

    function publicate() {

    }

    return (
        <div className={s.container}>
            <UserData subscribeBtn={false}>
                <span>Сегодня</span>
            </UserData>
            <div className={s.desc}>
                <form className={s.form} onSubmit={(e) => e.preventDefault()}>
                    <label className={s.name}>
                        Название: 
                        <input type="text" value={description.name} onInput={(e) => {
                            setDescription({...description, name: e.target.name});
                        }} />
                    </label>
                    <label className={s.descText}>
                        Описание:
                        <textarea 
                            cols="10" 
                            rows="7"
                            value={description.desc}
                            onInput={(e) => {
                                if (e.target.value.length >= 300) return;

                                setDescription({...description, desc: e.target.value});
                            }}
                        ></textarea>
                    </label>
                    <label className={s.taginput}>
                        Добавить тег:
                        <input type="text" value={newTag} onInput={(e) => setNewTag(e.target.value)} />
                        <button type='button' onClick={addTag}>Добавить</button>
                    </label>
                </form>
                <ul className={s.tags}>
                    {
                        tags.length ?
                        tags.map((tag) => <li key={tag} className={s.tag}>#{tag}</li>)
                            :
                        "пусто"
                    }
                </ul>
                <input className={s.file} type="file" accept="video/mp4" onChange={(e) => {
                    onVideoLoad(e.target.files[0].name);
                }}/>
                <button onClick={publicate} className={s.sbmt} type='submit'>Опубликовать</button>
            </div>
        </div>
    )
}
