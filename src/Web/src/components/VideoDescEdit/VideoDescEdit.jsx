import React, { useContext, useRef, useState } from 'react'
import s from './VideoDescEdit.module.css'
import UserData from '../UserData/UserData'
import { AuthContext } from '../../context';
import ShortsService from '../../API/shortsService';

export default function VideoDescEdit({onVideoLoad}) {
    const form = useRef();
    const {tokens, curUserData} = useContext(AuthContext);

    function publicate(e) {
        e.preventDefault();

        (async () => {
            const res = await ShortsService.publicateShort(tokens.accessToken, e.target);
            console.log(res);
            const json = await res.json();
            console.log(json);

            if (json.fileDownloadIsSuccess) {
                alert("Видео успешно загружено!");
                e.target.elements.Description.value = "";
                e.target.elements.File.value = "";
                e.target.elements.Title.value = "";
            } else {
                alert("что то пошло не так...");
            }
        })();
        console.dir(e.target);
    }

    return (
        <div className={s.container}>
            <UserData user={curUserData} subscribeBtn={false}>
                <span>Сегодня</span>
            </UserData>
            <div className={s.desc}>
                <form ref={form} className={s.form} onSubmit={publicate}>
                    <label className={s.name}>
                        Название: 
                        <input name='Title' type="text" />
                    </label>
                    <label className={s.descText}>
                        Описание:
                        <textarea cols="10" rows="7" name='Description'></textarea>
                    </label>
                    <input name='File' className={s.file} type="file" accept="video/mp4" onChange={(e) => {
                        onVideoLoad(e.target.files[0].name);
                    }}/>
                    <button className={s.sbmt} type='submit'>Опубликовать</button>
                </form>
            </div>
        </div>
    )
}
