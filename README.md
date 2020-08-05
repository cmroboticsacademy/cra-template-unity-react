# Unity-React-Template

## Installing
1) Install [npx](https://www.npmjs.com/package/npx) 
2) Open a terminal and go to a folder where you want to download the template.
3) Enter the command: `npx create-react-app --template @carnegie-mellon-robotics-academy/cra-template-react-app`

### Project Environment
This template installs a Unity Project along side of a react application. The root of the project is meant to be a workspace for your IDE.

## Getting Started with an example
### Building Unity
1) Open My_Unity_Project through Unity3D editor. You may need to upgrade or downgrade, that is fine.
2) Once open, add TestScene to your build settings.
3) Click build. Name the game "unity_game" and save it in the dist folder.
### Building the app
1) cd into the root of the project.
2) run `npm build` or `yarn build`  (if the build fails try removing node_models and re-install)
3) Once, complete, run `npm start` or `yarn start`