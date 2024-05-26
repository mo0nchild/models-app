import React from 'react';
import { Alert } from 'react-bootstrap';

import '@core/BootstrapFix.css'

interface ErrorInfoProps {
    message: string | null;
    onClose: () => void;
}
function ErrorInfoComponent({message, onClose}: ErrorInfoProps): React.JSX.Element {
    return (
        message == null ? <div></div> : <Alert dismissible onClose={onClose} variant='danger' 
            className='error-panel'>{ message }</Alert>
    )
}
export interface ErrorInfoHandler {
    setError: (message: string) => void
}
export const ErrorInfo = React.forwardRef<ErrorInfoHandler, {}>((_, ref) => {
    const [message, setMessage] = React.useState<string | null>(null);
    
    React.useImperativeHandle(ref, () => ({
        setError: (message) => setMessage(message)
    }))
    return (<ErrorInfoComponent onClose={() => setMessage(null)} message={message}/>)
})
