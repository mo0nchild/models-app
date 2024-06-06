import UserCard from '@components/UserCard';
import { Rating } from '@mui/material';
import React from 'react';
import StarIcon from '@mui/icons-material/Star'

export interface CommentInfo {
    user: { image: string, name: string };
    text: string;
    rating: number;
}
export default function CommentInfo({text, rating, user}: CommentInfo): React.JSX.Element {
    return (
    <div>
        <div style={commentHeaderStyle}>
            <UserCard imageUrl={user.image} name={user.name}/>
            <Rating value={rating} color={'#FFFFFF'} disabled
                emptyIcon={<StarIcon style={{ opacity: 0.8, color: '#AAA' }} fontSize="inherit" />}/>
        </div>
        <p style={{textAlign: 'start'}}>{text}</p>
    </div>
    )
}
const commentHeaderStyle: React.CSSProperties = {
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'space-between',
    margin: '0px 0px 10px',
    flexFlow: 'row wrap'
}