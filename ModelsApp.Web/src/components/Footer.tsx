import React from 'react';

export default function Footer(): React.JSX.Element {
    return (
    <footer style={footerStyle}>
        <div className="text-center p-3" style={{backgroundColor: 'rgba(0, 0, 0, 0.05)'}}>
          © 2024 Copyright:&nbsp;
          <a className="text-light" href="">YourModels.com</a>
        </div>
    </footer>
    )
}
const footerStyle: React.CSSProperties = {
	backgroundColor: '#242424', 
	color: '#FFF', 
	boxShadow: '0px 0px 20px #888'
}