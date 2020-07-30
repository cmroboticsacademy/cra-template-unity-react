import React from 'react';
import ReactDOM from 'react-dom';
import './style/styles.scss';

import UnityWorld from "./UnityWorld";

const Index = () => {
  
  return (<div><UnityWorld /></div>);
};
ReactDOM.render(<Index />, document.getElementById('root'));