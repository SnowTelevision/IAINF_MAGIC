using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GenerateWorkingHours : MonoBehaviour
{
    public TMP_InputField[] timeFields;
    public TMP_InputField[] totalTimeFields;
    public TMP_InputField randomMinuteInput;
    public TMP_InputField startHour;
    public TMP_InputField lunchHour;
    public TMP_InputField backHour;
    public TMP_InputField endHour;
    public TMP_InputField startMinute;
    public TMP_InputField lunchMinute;
    public TMP_InputField backMinute;
    public TMP_InputField endMinute;
    public TMP_InputField fiveDayTotalTimeField;

    public List<SingleDayHours> singleDayHours;
    public int start;
    public int lunch;
    public int back;
    public int end;
    public int randomMinute;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public int ToMinutes(int hour, int minute)
    {
        return (hour * 60 + minute);
    }

    public void GetInput()
    {
        randomMinute = Int32.Parse(randomMinuteInput.text);
        start = ToMinutes(Int32.Parse(startHour.text), Int32.Parse(startMinute.text));
        lunch = ToMinutes(Int32.Parse(lunchHour.text), Int32.Parse(lunchMinute.text));
        back = ToMinutes(Int32.Parse(backHour.text), Int32.Parse(backMinute.text));
        end = ToMinutes(Int32.Parse(endHour.text), Int32.Parse(endMinute.text));
    }

    public void FillOneDay()
    {
        GetInput();

        SingleDayHours newDayHours = new SingleDayHours();
        newDayHours.GetTimes(start + BetterRandom.betterRandom(-randomMinute, randomMinute),
                             lunch + BetterRandom.betterRandom(-randomMinute, randomMinute),
                             back + BetterRandom.betterRandom(-randomMinute, randomMinute),
                             end + BetterRandom.betterRandom(-randomMinute, randomMinute));

        timeFields[0].text = newDayHours.GetHoursDisplay();
        totalTimeFields[0].text = newDayHours.GetTotalTimeDisplay();
    }

    public void FillFiveDays()
    {
        singleDayHours = new List<SingleDayHours>();
        GetInput();

        for (int i = 0; i < 5; i++)
        {
            SingleDayHours newDayHours = new SingleDayHours();
            newDayHours.GetTimes(start + BetterRandom.betterRandom(-randomMinute, randomMinute),
                                 lunch + BetterRandom.betterRandom(-randomMinute, randomMinute),
                                 back + BetterRandom.betterRandom(-randomMinute, randomMinute),
                                 end + BetterRandom.betterRandom(-randomMinute, randomMinute));
            singleDayHours.Add(newDayHours);
        }

        int whileStopper = 999999999;

        while ((GetFiveDayTotalMinutes() < (34.5 * 60) || GetFiveDayTotalMinutes() > (35.5 * 60)) && whileStopper > 0)
        {
            if (GetFiveDayTotalMinutes() < (34.5 * 60))
            {
                AdjustDayTimes(true);
            }
            if (GetFiveDayTotalMinutes() > (35.5 * 60))
            {
                AdjustDayTimes(false);
            }

            whileStopper--;
        }

        print(whileStopper);

        for (int i = 0; i < 5; i++)
        {
            timeFields[i].text = singleDayHours[i].GetHoursDisplay();
            totalTimeFields[i].text = singleDayHours[i].GetTotalTimeDisplay();
        }

        int fiveDayTotalHours = GetFiveDayTotalMinutes() / 60;

        fiveDayTotalTimeField.text = fiveDayTotalHours.ToString() + "小时" + (GetFiveDayTotalMinutes() - fiveDayTotalHours * 60).ToString() + "分钟";

    }

    public void AdjustDayTimes(bool increase)
    {
        int timeStampIndex = BetterRandom.betterRandom(0, 3);
        int dayIndex = BetterRandom.betterRandom(0, 4);

        if (increase)
        {
            if (timeStampIndex == 0)
            {
                singleDayHours[dayIndex].hours[timeStampIndex] -= BetterRandom.betterRandom(0, randomMinute);
            }
            else if (timeStampIndex == 1)
            {
                if (!ValidTimeStamp(dayIndex, 2, 1, 80))
                {
                    return;
                }

                singleDayHours[dayIndex].hours[timeStampIndex] += BetterRandom.betterRandom(0, randomMinute);
            }
            else if (timeStampIndex == 2)
            {
                if (!ValidTimeStamp(dayIndex, 2, 1, 80))
                {
                    return;
                }

                singleDayHours[dayIndex].hours[timeStampIndex] -= BetterRandom.betterRandom(0, randomMinute);
            }
            else if (timeStampIndex == 3)
            {
                singleDayHours[dayIndex].hours[timeStampIndex] += BetterRandom.betterRandom(0, randomMinute);
            }
        }
        else
        {
            if (timeStampIndex == 0)
            {
                if (!ValidTimeStamp(dayIndex, 1, 0, ((lunch - start) * 0.8f)))
                {
                    return;
                }

                singleDayHours[dayIndex].hours[timeStampIndex] += BetterRandom.betterRandom(0, randomMinute);
            }
            else if (timeStampIndex == 1)
            {
                if (!ValidTimeStamp(dayIndex, 1, 0, ((lunch - start) * 0.8f)) || ValidTimeStamp(dayIndex, 2, 1, ((back - lunch) * 1.1f)))
                {
                    return;
                }

                singleDayHours[dayIndex].hours[timeStampIndex] -= BetterRandom.betterRandom(0, randomMinute);
            }
            else if (timeStampIndex == 2)
            {
                if (!ValidTimeStamp(dayIndex, 1, 0, ((end - back) * 0.8f)) || ValidTimeStamp(dayIndex, 2, 1, ((back - lunch) * 1.1f)))
                {
                    return;
                }

                singleDayHours[dayIndex].hours[timeStampIndex] += BetterRandom.betterRandom(0, randomMinute);
            }
            else if (timeStampIndex == 3)
            {
                if (!ValidTimeStamp(dayIndex, 1, 0, ((end - back) * 0.8f)))
                {
                    return;
                }

                singleDayHours[dayIndex].hours[timeStampIndex] -= BetterRandom.betterRandom(0, randomMinute);
            }
        }
    }

    public bool ValidTimeStamp(int dayIndex, int end, int start, float range)
    {
        if (singleDayHours[dayIndex].hours[end] - singleDayHours[dayIndex].hours[start] < range)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public int GetFiveDayTotalMinutes()
    {
        return (singleDayHours[0].GetTotalMinutes() +
                singleDayHours[1].GetTotalMinutes() +
                singleDayHours[2].GetTotalMinutes() +
                singleDayHours[3].GetTotalMinutes() +
                singleDayHours[4].GetTotalMinutes());
    }
}

public class SingleDayHours
{
    public List<int> hours = new List<int>();

    public void GetTimes(int start, int lunch, int back, int end)
    {
        hours.Add(start);
        hours.Add(lunch);
        hours.Add(back);
        hours.Add(end);
    }

    public int GetTotalMinutes()
    {
        return ((hours[1] - hours[0]) + (hours[3] - hours[2]));
    }

    public string GetTotalTimeDisplay()
    {
        return (GetTotalMinutes() / 60).ToString() + "小时" + GetRemainedMinutes(GetTotalMinutes()).ToString() + "分钟";
    }

    public string GetHoursDisplay()
    {
        string displayString = "";

        for (int i = 0; i < hours.Count; i++)
        {
            if (i == 0)
            {
                displayString += GetTimeDisplay(hours[i]);
            }
            else if (i == 1)
            {
                displayString += " - " + GetTimeDisplay(hours[i]);
            }
            else if (i == 2)
            {
                displayString += ", " + GetTimeDisplay(hours[i]);
            }
            else if (i == 3)
            {
                displayString += " - " + GetTimeDisplay(hours[i]);
            }
        }

        return displayString;
    }

    public string GetTimeDisplay(int minutes)
    {
        int hour = GetHours(minutes);
        int minute = GetRemainedMinutes(minutes);
        string timeString = "";
        string minuteString = "";

        if (minute < 10)
        {
            minuteString = "0" + minute.ToString();
        }
        else
        {
            minuteString = minute.ToString();
        }

        if (hour < 12)
        {
            timeString = hour.ToString() + ":" + minuteString + "am";
        }
        else
        {
            if (hour == 12)
            {
                timeString = hour.ToString() + ":" + minuteString + "pm";
            }
            else
            {
                timeString = (hour - 12).ToString() + ":" + minuteString + "pm";
            }
        }

        return timeString;
    }

    public int GetHours(int minutes)
    {
        return (minutes / 60);
    }

    public int GetRemainedMinutes(int minutes)
    {
        return (minutes - (GetHours(minutes) * 60));
    }
}
