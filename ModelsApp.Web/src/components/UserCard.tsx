import React from 'react';

export interface UserCardProps {
    imageUrl: string, name: string,
    onClick?: () => void
}
export default function UserCard({name, imageUrl, onClick}: UserCardProps): React.JSX.Element {
    return (
    <div style={userCardStyle} onClick={() => onClick?.()}>
        <img src={imageUrl} alt='userCard' style={userImageStyle}/>
        <span style={{marginLeft: '16px', fontSize: '16px'}}>{name}</span>
    </div>
    )
}
const userCardStyle: React.CSSProperties = {
    display: 'flex',
    width: '100%',
    alignItems: 'center',
    cursor: 'pointer'
}
const userImageStyle: React.CSSProperties = {
    width: '50px', height: '50px',
    objectFit: 'contain'
}