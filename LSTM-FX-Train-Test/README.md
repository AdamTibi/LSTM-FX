# Training your model (Running the Code)
---
This code is intended to run in order:
## Download a Data Set
I left a sample GBPUSD data set `gbpusd.csv` in `./LSTM-FX-Train-Test/data`, however, you may use any Forex pair as long as the header (first line) is this:
```
Date,Open,High,Low,Close,Volume
```
And the rest of the lines have data following this format:
```
2010-01-01 00:01:00,1.61670,1.61670,1.61670,1.61670,24
```
## Setting Up the Variables (Parameters and Hyperparameters)
Edit `common_variables.py` and set the `batch_size`, `window_size`, `validation_size`, `test_size`. Make sure any other Jupyter Notebook is closed to ensure they pick the latest `common_variables.py`.

Every time you modify the `common_variables.py` you will have to start again from this step.

## Preparing and Splitting the Data Set
Open `prep_and_split.ipynb` with Jupyter Notebook and execute it.
## Training Your Model
Open `train_model.ipynb` and modify the epoch number. Your model is generated in `./LSTM-FX-Train-Test/models` and your scaler in `./LSTM-FX-Train-Test/scalers`.
## Testing Your Model
Open `test_model.ipynb` model and run it. The will test the model for a single time unit prediction.
## Multiple Predictions
Open `multi_pred_model.ipynb` and configure:
- pred_interval: This is to try to predict multiple times separated by this interval.
- pred_size: This is the length of the prediction, in other words, how many units (minutes in this case) do you want to predict into the future.