import { Rating } from '@mui/material';
import StarIcon from '@mui/icons-material/Star'
import React from 'react'
import { Image } from 'react-bootstrap';

export interface CatalogCardProps {
    name: string, rating: number,
    imageUrl: string, guid: string,
    onCardClicked: () => void;
}
export default function CatalogCard(props: CatalogCardProps): React.JSX.Element {
    const { name, rating, imageUrl, onCardClicked } = props;
    return (
        <div className='card-info' style={catalogCardStyle} onClick={() => onCardClicked()}>
            <Image src={imageUrl} rounded style={{
                objectFit: 'contain',
                width: '100%'
            }} />
            <span style={{margin: '10px 0px', color: '#FFF'}}>{name}</span>
            <Rating value={rating} color={'#FFFFFF'} disabled
                emptyIcon={<StarIcon style={{ opacity: 0.8, color: '#AAA' }} fontSize="inherit" />}/>
        </div>
    )
}
const catalogCardStyle: React.CSSProperties = {
    width: '100%',
    display: 'flex',
    flexFlow: 'column',
    alignItems: 'center',
    padding: '20px',
}