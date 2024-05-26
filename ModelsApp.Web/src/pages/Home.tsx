import React from 'react';
import { useNavigate } from 'react-router-dom';

export default function Home(): React.JSX.Element {
    const navigator = useNavigate();
    return (
    <div>
        Hello
        <button onClick={() => {
            const test = () => navigator('/login')
            test();
            console.log('test')
        }}>test</button>
    </div>
    );
}