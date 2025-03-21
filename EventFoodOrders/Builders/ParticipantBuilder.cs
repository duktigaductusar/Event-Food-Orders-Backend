using EventFoodOrders.Entities;
using EventFoodOrders.Utilities;

namespace EventFoodOrders.Builders
{
    public class ParticipantBuilder
    {
        private Participant _participant;

        public ParticipantBuilder()
        {
            _participant = new();
        }

        public ParticipantBuilder(Participant participant)
        {
            _participant = participant;
            SetResponse(participant.Response);
        }

        public Participant BuildParticipant()
        {
            return _participant;
        }

        public Participant SetParticipantId(Guid participantId)
        {
            _participant.Id = participantId;
            return _participant;
        }

        public Participant SetUserId(Guid userId)
        {
            _participant.UserId = userId;
            return _participant;
        }

        public Participant SetEvent(Event e)
        {
            _participant.EventId = e.Id;
            return _participant;
        }

        public Participant SetResponse(string response)
        {
            if (Utility.PossibleResponses.Where(r => r == response).Any())
            {
                _participant.Response = response;
            }
            else
            {
                _participant.Response = ReType.Pending;
            }

            return _participant;
        }

        public Participant SetWantsMeal(bool wantsMeal)
        {
            _participant.WantsMeal = wantsMeal;
            return _participant;
        }

        public Participant SetAllergies(string[] allergies)
        {
            _participant.Allergies = allergies;
            return _participant;
        }

        public Participant SetPreferences(string[] preferences)
        {
            _participant.Preferences = preferences;
            return _participant;
        }
    }
}
