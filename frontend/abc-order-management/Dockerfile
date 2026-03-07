# node version 22
FROM node:22

# working directory
WORKDIR /app

# copy package.json and install dependencies
COPY package*.json ./

RUN npm install

# disable angular analytics
ENV NG_CLI_ANALYTICS=false

# copy the rest of the application code
COPY . .

# port 
EXPOSE 4200

# run dev
CMD ["npm", "start", "--", "--host", "0.0.0.0", "--poll", "2000"]