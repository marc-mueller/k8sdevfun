using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DevFun.Web.Ui.Tests
{
    [TestClass]
    public class RandomJokeTests : WebUITestBase
    {
        [TestMethod]
        public async Task DevFun_GetNextRandomJoke_ShowNewJoke()
        {
            // Arrange
            var app = await Launch().ConfigureAwait(false);
            var randomJokesPanel = await (await app.NavigateToRandomJokes().ConfigureAwait(false)).VerifyAlternateUrl().ConfigureAwait(false);

            var oldJoke = await randomJokesPanel.GetJokeText().ConfigureAwait(false);
            var newJoke = string.Empty;

            // Act
            int counter = 0;
            for (counter = 0; counter < 10; counter++)
            {
                randomJokesPanel = await (await randomJokesPanel.GotoHome().ConfigureAwait(false)).VerifyAlternateUrl().ConfigureAwait(false);
                newJoke = await randomJokesPanel.GetJokeText().ConfigureAwait(false);
                if (!newJoke.Equals(oldJoke))
                {
                    break;
                }
            }

            // Assert
            
            Assert.IsNotNull(newJoke);
            Assert.IsTrue(counter < 3);
            Assert.AreNotEqual(oldJoke, newJoke);
        }

        [TestMethod]
        public async Task DevFun_GotoAbout_ShowAboutInfo()
        {
            // Arrange
            var app = await Launch().ConfigureAwait(false);
            var randomJokesPanel = await (await app.NavigateToRandomJokes().ConfigureAwait(false)).VerifyAlternateUrl().ConfigureAwait(false);

            // Act
            var aboutPanel = await randomJokesPanel.GotoAbout().ConfigureAwait(false);

            // Assert
            var text = await aboutPanel.GetAboutText().ConfigureAwait(false);
            Assert.IsNotNull(text);
            Assert.IsFalse(string.IsNullOrWhiteSpace(text));
        }

        [TestMethod]
        public async Task DevFun_NavigateMultipleTimes_ShowJoke()
        {
            // Arrange
            var app = await Launch().ConfigureAwait(false);
            var randomJokesPanel = await (await app.NavigateToRandomJokes().ConfigureAwait(false)).VerifyAlternateUrl().ConfigureAwait(false);

            var oldJoke = await randomJokesPanel.GetJokeText().ConfigureAwait(false);
            var newJoke = string.Empty;

            // Act
            int counter = 0;
            for (counter = 0; counter < 10; counter++)
            {
                randomJokesPanel = await (await ( await ( await ( await (await ( await (await randomJokesPanel
                .GotoHome().ConfigureAwait(false)).VerifyAlternateUrl().ConfigureAwait(false))
                .GotoAbout().ConfigureAwait(false))
                .GotoHome().ConfigureAwait(false)).VerifyAlternateUrl().ConfigureAwait(false))
                .GotoAbout().ConfigureAwait(false))
                .GotoHome().ConfigureAwait(false)).VerifyAlternateUrl().ConfigureAwait(false);
                newJoke = await randomJokesPanel.GetJokeText().ConfigureAwait(false);
                if (!newJoke.Equals(oldJoke))
                {
                    break;
                }
            }

            // Assert

            Assert.IsNotNull(newJoke);
            Assert.IsTrue(counter < 3);
            Assert.AreNotEqual(oldJoke, newJoke);
        }
    }
}