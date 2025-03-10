# -*- coding: utf-8 -*-
from os import truncate
from tests.performance_lighthouse import run_test as lighthouse_perf_run_test
from models import Rating
import datetime
import tests.energy_efficiency_carbon_percentiles as energy_efficiency_carbon_percentiles
import tests.energy_efficiency_carbon_percentiles2021 as energy_efficiency_carbon_percentiles_2021
from tests.utils import *
import gettext
_local = gettext.gettext

# Code below is built from: https://gitlab.com/wholegrain/carbon-api-2-0

KWG_PER_GB = 1.805
RETURNING_VISITOR_PERCENTAGE = 0.75
FIRST_TIME_VIEWING_PERCENTAGE = 0.25
PERCENTAGE_OF_DATA_LOADED_ON_SUBSEQUENT_LOAD = 0.02
CARBON_PER_KWG_GRID = 475
# PER_KWG_RENEWABLE = 33.4
# PERCENTAGE_OF_ENERGY_IN_DATACENTER = 0.1008
# PERCENTAGE_OF_ENERGY_IN_TRANSMISSION_AND_END_USER = 0.8992
# CO2_GRAMS_TO_LITRES = 0.5562


def adjust_data_transfer(transfer_bytes):
    return (transfer_bytes * RETURNING_VISITOR_PERCENTAGE) + (PERCENTAGE_OF_DATA_LOADED_ON_SUBSEQUENT_LOAD * transfer_bytes * FIRST_TIME_VIEWING_PERCENTAGE)


def energy_consumption(bytesAdjusted):
    return bytesAdjusted * (KWG_PER_GB / 1073741824)


def get_co2_grid(energy):
    return energy * CARBON_PER_KWG_GRID


# def get_co2_renewable(energy):
#     return ((energy * PERCENTAGE_OF_ENERGY_IN_DATACENTER) * CARBON_PER_KWG_RENEWABLE) + ((energy * PERCENTAGE_OF_ENERGY_IN_TRANSMISSION_AND_END_USER) * CARBON_PER_KWG_GRID)


def convert_2_co2(transfer_bytes):
    bytesAdjusted = adjust_data_transfer(transfer_bytes)
    energy = energy_consumption(bytesAdjusted)
    co2Grid = get_co2_grid(energy)
    # co2Renewable = get_co2_renewable(energy)
    return co2Grid


def cleaner_than(co2, year='current'):
    # This array needs to be updated periodically with new data. This was
    # originally calculated with a database query but that was too slow at
    # scale. We can look in to writing a cron job that will generate and export
    # from the database once a month, that is then loaded in this file.

    percentiles = False
    if year == '2021':
        percentiles = energy_efficiency_carbon_percentiles_2021.get_percentiles()
    else:
        percentiles = energy_efficiency_carbon_percentiles.get_percentiles()
    position = 0
    # this should always be exactly 100 number of values
    for item in percentiles:
        position += 1
        if(co2 < item):
            return (100 - position) / 100
    return 0


def format_bytes(size):
    # 2**10 = 1024
    power = 2**10
    n = 0
    power_labels = {0: '', 1: 'k', 2: 'm', 3: 'g', 4: 't'}
    while size > power:
        size /= power
        n += 1
    return size, power_labels[n]+'b'


def run_test(_, langCode, url):
    """
    Analyzes URL with Website Carbon Calculator API.
    API documentation: https://api.websitecarbon.com
    https://gitlab.com/wholegrain/carbon-api-2-0
    """

    language = gettext.translation(
        'energy_efficiency_websitecarbon', localedir='locales', languages=[langCode])
    language.install()
    _local = language.gettext

    print(_local("TEXT_RUNNING_TEST"))

    print(_('TEXT_TEST_START').format(
        datetime.datetime.now().strftime('%Y-%m-%d %H:%M:%S')))

    result_dict = {}
    lighthouse_perf_result = lighthouse_perf_run_test(_, langCode, url, True)

    transfer_bytes = lighthouse_perf_result[1]['total-byte-weight']
    result_dict['total-byte-weight'] = transfer_bytes
    #print('transfer bytes: {0}'.format(transfer_bytes))

    co2 = convert_2_co2(transfer_bytes)
    result_dict['co2'] = co2

    cleaner = cleaner_than(co2) * 100
    cleaner_2021 = cleaner_than(co2, '2021') * 100
    result_dict['cleaner_than'] = cleaner

    review = ''

    points = float("{0:.2f}".format((5 * (cleaner / 100))))

    # handicap points
    if cleaner >= 95:
        points = 5.0

    if cleaner >= 90:
        review = _local("TEXT_WEBSITE_IS_VERY_GOOD")
    elif cleaner >= 70:
        review = _local("TEXT_WEBSITE_IS_GOOD")
    elif cleaner >= 50:
        review = _local("TEXT_WEBSITE_IS_OK")
    elif cleaner >= 30:
        review = _local("TEXT_WEBSITE_IS_BAD")
    elif cleaner < 30:
        review = _local("TEXT_WEBSITE_IS_VERY_BAD")

    review += _local("TEXT_GRAMS_OF_CO2").format(round(co2, 2))
    review += _local("TEXT_BETTER_THAN").format(int(cleaner),
                                                energy_efficiency_carbon_percentiles.get_generated_date())
    review += _local("TEXT_BETTER_THAN").format(int(cleaner_2021),
                                                energy_efficiency_carbon_percentiles_2021.get_generated_date())

    transfer_info = format_bytes(transfer_bytes)
    review += _local("TEXT_TRANSFER_SIZE").format(transfer_info[0], transfer_info[1]
                                                  )
    # if 'false' in green.lower():
    # review += _local("TEXT_GREEN_ENERGY_FALSE")
    # elif 'true' in green.lower():
    # review += _local("TEXT_GREEN_ENERGY_TRUE")

    rating = Rating(_)
    rating.set_overall(points, review)
    # we use this line to recalibrate percentiles (See carbon-rating.py), comment out line above also
    #rating.set_overall(points, '{0}'.format(co2))

    print(_('TEXT_TEST_END').format(
        datetime.datetime.now().strftime('%Y-%m-%d %H:%M:%S')))

    return (rating, result_dict)
