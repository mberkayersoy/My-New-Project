# Psycho Bubbles
If you played a lot of web games like me when you were little, you will remember the bubble trouble game. You know, a 2D game where you manage the devil character and destroy them by hitting the balls. Psycho Bubbles is exactly such a game, but with 3D and multiplayer :)

## Contents
- [Project Description](#project-description)
- [Installation](#installation)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)


## Project Description

I developed Psycho Bubbles as my graduation project. The game is played in 3D FPS style. This is not a finished game project, but if you want to make a multiplayer game, you can customize it as you wish, making it the skeleton of your project or you can use it directly.

![login](https://github.com/mberkayersoy/Psycho_Bubbles/assets/76611569/190d2828-3ebb-4704-add5-de6a773fa426)
![register](https://github.com/mberkayersoy/Psycho_Bubbles/assets/76611569/75df4619-87da-4ccf-bda6-fdd6d2d77497)
![Uploading requestt.png…]()![register](https://github.com/mberkayersoy/Psycho_Bubbles/assets/76611569/63ba3682-690a-435e-96b2-7687b92ee289)
![privchat](https://github.com/mberkayersoy/Psycho_Bubbles/assets/76611569/77ceeb05-3093-47a5-a99a-8ad22f65ffac)
![firebasechat](https://github.com/mberkayersoy/Psycho_Bubbles/assets/76611569/59b44b2c-7069-4e0f-ae49-86939e749206)
![insideroom](https://github.com/mberkayersoy/Psycho_Bubbles/assets/76611569/026f6dc0-5e0a-459f-ba82-613e7f206473)
![abilities](https://github.com/mberkayersoy/Psycho_Bubbles/assets/76611569/33640a6e-28e6-498d-ba77-51d56de8fafb)
![jumped](https://github.com/mberkayersoy/Psycho_Bubbles/assets/76611569/1b6d2755-caa8-4709-a374-27208d4f2f8f)


## Installation

First, download the repository and open it in Unity. Then create your own App Id PUN and App Id Chat keys by opening a photon account.

### Photon PUN Configuration

![photonweb](https://github.com/mberkayersoy/Psycho_Bubbles/assets/76611569/b05166a6-3123-4f11-8939-6b8053a73506)
Then use the keys you created in Unity.
![photonıDs](https://github.com/mberkayersoy/Psycho_Bubbles/assets/76611569/591b361d-3858-46e7-b4e0-257c59419ceb)

That's all you need for Photon.

### Firebase Configuration

The Assets folder contains the 'GoogleService-Info.plist' file. Create your own Firestore Database in Firebase. If you want, change the 'GoogleService-Info.plist' file completely or write your own database information in the relevant places.

![firebase](https://github.com/mberkayersoy/Psycho_Bubbles/assets/76611569/cc294936-1f2c-48d8-b684-e0a2999a5c6a)

Please replace the "YOUR_CLIENT_ID", "YOUR_REVERSED_CLIENT_ID", "YOUR_API_KEY", "YOUR_GCM_SENDER_ID", "YOUR_PROJECT_ID", "YOUR_STORAGE_BUCKET", "YOUR_GOOGLE_APP_ID" with your own placeholder's actual placeholder data such as "YOUR_GOOGLE_APP_ID" and "YOUR_DATA" placeholder of your project. This way, you're updating the file and keeping your confidential database credentials.

#### If you get stuck anywhere, please don't hesitate to contact me. mail: mberkayersoy@gmail.com

## License

MIT License

Copyright (c) 2023 Muhammet Berkay Ersoy

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

