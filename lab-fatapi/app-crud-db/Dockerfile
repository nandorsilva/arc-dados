FROM python:3.10

 
# Create the home directory
ENV APP_HOME=/home/app/api
RUN mkdir -p $APP_HOME
WORKDIR $APP_HOME
# 

# install
COPY . $APP_HOME
RUN pip install --no-cache-dir --upgrade -r requirements.txt
RUN pip install -e .


# 
CMD ["uvicorn","app.main:app","--host=0.0.0.0","--port=8000","--reload"]

