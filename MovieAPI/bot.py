from urllib.request import urlopen as uReq
import bs4
from bs4 import BeautifulSoup
import requests
import pandas as pd
from tqdm import tqdm
import numpy as np


def download_img(img_url, title):
    my_url = f'https://image.tmdb.org/t/p/w342/{img_url}'
    response = requests.get(my_url, stream=True)
    file_size = int(response.headers.get("Content-Length", 0))
    file_name = img_url.split('/')[-1].split('.')[0] + "_" + title.replace(' ', '_') + '.jpg'
    with open('posters/' + file_name, "wb") as f:
        for data in response.iter_content(1024):
            f.write(data)
    return file_name

def get_data(id):
    try:
        movie_response = requests.get(f'{API_URL}/{id}{API_KEY}').json()
        title = movie_response['title']
        overview = movie_response['overview']
        poster_path = movie_response['poster_path']
        poster_path = download_img(poster_path, title)

        video_response = requests.get(f'{API_URL}/{id}/videos{API_KEY}').json()
        video_key = None
        for vid in video_response['results']:
            if vid['type'] == 'Trailer':
                video_key = vid['key']
        
        if video_key is None:
            return pd.Series({'id': np.nan, 'title': np.nan, 'overview': np.nan, 'poster_path': np.nan, 'video_key': np.nan})

        return pd.Series({'id': id, 'title': title, 'overview': overview, 'poster_path': poster_path, 'video_key': video_key})
    except:
        return pd.Series({'id': np.nan, 'title': np.nan, 'overview': np.nan, 'poster_path': np.nan, 'video_key': np.nan})


links_df = pd.read_csv('ml-latest-small/links.csv')


API_KEY = '?api_key=9fe15a0c0fc071b3fab1b7cb6cf61dee'
API_URL = 'https://api.themoviedb.org/3/movie/'

tqdm.pandas()

results_df = links_df['tmdbId'].progress_apply(get_data)
results_df.dropna(inplace=True)
results_df.to_csv('results.csv', index=False)





