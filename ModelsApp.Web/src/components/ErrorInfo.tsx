import React from 'react';
import { Alert } from 'react-bootstrap';

import '@styles/BootstrapFix.css'

interface ErrorInfoProps {
    readonly message: string | null;
    readonly onClose: () => void;
}
function ErrorInfoComponent({message, onClose}: ErrorInfoProps): React.JSX.Element {
    return (
        message == null ? <div></div> : <Alert dismissible onClose={onClose} variant='danger' 
            className='error-panel'>{ message }</Alert>
    )
}
export interface ErrorInfoHandler {
    readonly setError: (message: string) => void,
    readonly closeError: () => void
}
export const ErrorInfo = React.forwardRef<ErrorInfoHandler, {}>((_, ref) => {
    const [message, setMessage] = React.useState<string | null>(null);
    
    React.useImperativeHandle(ref, () => ({
        setError: (message) => setMessage(message),
        closeError: () => setMessage(null)
    }))
    return (<ErrorInfoComponent onClose={() => setMessage(null)} message={message}/>)
})
