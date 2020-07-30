import React, { useEffect, useState } from 'react';
import Unity, { UnityContent } from 'react-unity-webgl';
import { registerCommunicationHandler } from "./unity_api";
import './UnityWorld.css';

const UnityWorld = () => {

  const [unity, setUnity] = useState(null);
  useEffect(() => {
    let unityContent = new UnityContent(
      `unity_game/Build/unity_game.json`,
      `unity_game/Build/UnityLoader.js`
    );
    unityContent.on("loaded", () => {
      console.log('loaded');
    })
    setUnity(unityContent);
  }, []);

  useEffect(() => {
    if (unity) {
      console.log("unity", unity);
      registerCommunicationHandler(unity);
    }
  }, [unity])

  return (
    <div className="unity-container" >
      {unity &&
        <Unity unityContent={unity} />
      }
    </div>
  );
};

export default UnityWorld;
