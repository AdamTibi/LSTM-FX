# MIT License
# Copyright (c) 2020 Adam Tibi (https://linkedin.com/in/adamtibi/ , https://adamtibi.net)

batch_size = 32
window_size = int(256) # must be a multiple of batch_size
validation_size = 8192 * batch_size # must be a multiple of batch_size
test_size = 8192 * batch_size # must be a multiple of batch_size
ma_periods = 14 # Simple Moving Average periods length
ticker = 'gbpusd' # Your data file name without extention
start_date = '2010-01-01' # Ignore any data in the file prior to this date
seed = 42 # An arbitrary value to make sure your seed is the same
model_path = f'models/{ticker}-{batch_size}-{window_size}-{ma_periods}'
scaler_path = f'scalers/{ticker}-{batch_size}-{window_size}-{ma_periods}.bin'
full_time_series_path = f'data/{ticker}.csv'
train_time_series_path = f'data/{ticker}-train.csv'
validate_time_series_path = f'data/{ticker}-validate.csv'
test_time_series_path = f'data/{ticker}-test.csv'